import { FC, useState, useEffect, useContext } from 'react';
import { Link } from 'react-router-dom';
import { useForm, SubmitHandler } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';

import { AuthContext } from '../../context/AuthContext';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';

import { AuthApi } from '../../services/AuthApi';

import Style from './AuthPage.module.css';

type LoginType = {
  email: string;
  password: string;
};

const loginSchema = yup
  .object({
    email: yup.string().email().required('Email is required'),
    password: yup
      .string()
      //.min(4)
      //.matches(/(?=.*\d)(?=.*[@$!%*?&])/,'Must include numbers and special characters',)
      .required('Password is required'),
  })
  .required();

type ButtonState = 'default' | 'invalid' | 'valid';

const AuthPage: FC = () => {
  const { setIsAuth } = useContext(AuthContext);

  const [buttonState, setButtonState] = useState<ButtonState>('default');

  const {
    register,
    handleSubmit,
    formState: { isValid, errors },
  } = useForm<LoginType>({
    mode: 'onChange',
    resolver: yupResolver(loginSchema),
  });

  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  const onSubmit: SubmitHandler<LoginType> = (data) => {
    async function fetchData() {
      const tokenResponse = await AuthApi.login(data.email, data.password);
      if (tokenResponse.data && !tokenResponse.error) {
        const token = tokenResponse.data;
        const tokenJsonData = JSON.stringify(token);
        localStorage.setItem('session', tokenJsonData);
        setIsAuth(true);
      } else {
        setErrorMessage([...errorMessage, tokenResponse.error as string]);
      }
    }

    fetchData();
  };

  useEffect(() => {
    if (errors.email !== undefined || errors.password !== undefined) {
      setButtonState('invalid');
    } else if (isValid) {
      setButtonState('valid');
    }
  }, [isValid, errors]);

  return (
    <>
      <div className={Style.container}>
        <div className={Style.block}>
          <h3>Войдите в аккаунт</h3>
          <form onSubmit={handleSubmit(onSubmit)} className={Style.form}>
            <div className={Style.inputWrapper}>
              <label className={Style.label}>Электронная почта *</label>
              <input className={Style.input} {...register('email')} />
              {errors?.email && (
                <p className={Style.validationError}>{errors.email.message}</p>
              )}
            </div>
            <div className={Style.inputWrapper}>
              <label className={Style.label}>Пароль *</label>
              <input
                type="password"
                className={Style.input}
                {...register('password')}
              />
              {errors?.password && (
                <p className={Style.validationError}>
                  {errors.password.message}
                </p>
              )}
            </div>
            <button
              className={`${Style.loginBtn} ${Style[buttonState]}`}
              type="submit"
            >
              Войти
            </button>
          </form>
          <Link className={Style.createBtn} to="/user/create">
            Создать аккаунт
          </Link>
        </div>
      </div>
      <ListErrorPopUp messages={errorMessage} />
    </>
  );
};

export default AuthPage;

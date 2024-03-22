import { FC, useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { useForm, SubmitHandler } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';

import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';

import { AuthApi } from '../../services/AuthApi';

import Style from './UserCreatePage.module.css';

type CreateUserType = {
  email: string;
  password: string;
  confirmPassword: string;
};

const userCreateSchema = yup
  .object({
    email: yup.string().email().required('Email is required'),
    password: yup
      .string()
      //.min(4)
      //.matches(/(?=.*\d)(?=.*[@$!%*?&])/,'Must include numbers and special characters',)
      .required('Password is required'),
    confirmPassword: yup
      .string()
      .required('Password is required')
      .oneOf([yup.ref('password')], 'Your passwords do not match.'),
  })
  .required();

type ButtonState = 'default' | 'invalid' | 'valid';

const UserCreatePage: FC = () => {
  const [buttonState, setButtonState] = useState<ButtonState>('default');

  const {
    register,
    handleSubmit,
    formState: { isValid, errors },
  } = useForm<CreateUserType>({
    mode: 'onChange',
    resolver: yupResolver(userCreateSchema),
  });

  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  const onSubmit: SubmitHandler<CreateUserType> = (data) => {
    console.log(data);
    async function fetchData() {}

    fetchData();
  };

  useEffect(() => {
    if (
      errors.email !== undefined ||
      errors.password !== undefined ||
      errors.confirmPassword !== undefined
    ) {
      setButtonState('invalid');
    } else if (isValid) {
      setButtonState('valid');
    }
  }, [isValid, errors]);

  return (
    <>
      <div className={Style.container}>
        <h3>Создайте аккаунт</h3>
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
              <p className={Style.validationError}>{errors.password.message}</p>
            )}
          </div>
          <div className={Style.inputWrapper}>
            <label className={Style.label}>Подтвердите пароль *</label>
            <input
              type="password"
              className={Style.input}
              {...register('confirmPassword')}
            />
            {errors?.confirmPassword && (
              <p className={Style.validationError}>
                {errors.confirmPassword.message}
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
        <Link to="/auth">Войти в аккаунт</Link>
      </div>
      <ListErrorPopUp messages={errorMessage} />
    </>
  );
};

export default UserCreatePage;

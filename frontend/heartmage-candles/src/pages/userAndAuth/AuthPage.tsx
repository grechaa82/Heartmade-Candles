import { FC, useState, useEffect } from 'react';
import { Link } from 'react-router-dom';

import Input from '../../components/shared/Input';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';

import { AuthApi } from '../../services/AuthApi';

import Style from './AuthPage.module.css';

const AuthPage: FC = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    async function fetchData() {
      const tokenResponse = await AuthApi.login(email, password);
      if (tokenResponse.data && !tokenResponse.error) {
        localStorage.setItem('token', tokenResponse.data);
      } else {
        setErrorMessage([...errorMessage, tokenResponse.error as string]);
      }
    }

    fetchData();
  };

  return (
    <>
      <div className={Style.container}>
        <h3>Войдите в аккаунт</h3>
        <form onSubmit={handleSubmit} className={Style.form}>
          <Input
            label="Электронная почта"
            required
            value={email}
            onChange={setEmail}
          />
          <Input
            label="Пароль"
            required
            value={password}
            onChange={setPassword}
          />
          <button className={Style.loginBtn} type="submit">
            Войти
          </button>
        </form>
        <Link className={Style.createBtn} to="/user/create">
          Создать аккаунт
        </Link>
      </div>
      <ListErrorPopUp messages={errorMessage} />
    </>
  );
};

export default AuthPage;

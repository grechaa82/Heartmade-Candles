import { FC, useState } from 'react';

import Input from '../../components/shared/Input';

import { AuthApi } from '../../services/AuthApi';

import Style from './AuthPage.module.css';

const AuthPage: FC = () => {
  const [login, setLogin] = useState('');
  const [password, setPassword] = useState('');

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    async function fetchData() {
      try {
        const token = await AuthApi.login(login, password);
        localStorage.setItem('token', token);
      } catch (error: any) {
        console.error('Произошла ошибка при загрузке данных:', error);
      }
    }

    fetchData();
  };

  return (
    <div className={Style.container}>
      <form onSubmit={handleSubmit} className={Style.form}>
        <Input label="Логин" required value={login} onChange={setLogin} />
        <Input label="Пароль" required value={password} onChange={setPassword} />
        <button className={Style.loginBtn} type="submit">
          Войти
        </button>
      </form>
    </div>
  );
};

export default AuthPage;

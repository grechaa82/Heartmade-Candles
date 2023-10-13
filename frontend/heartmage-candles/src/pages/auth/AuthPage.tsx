import { FC, useState } from 'react';

import Input from '../../components/shared/Input';
import ListErrorPopUp from '../../modules/constructor/ListErrorPopUp';

import { AuthApi } from '../../services/AuthApi';

import Style from './AuthPage.module.css';

const AuthPage: FC = () => {
  const [login, setLogin] = useState('');
  const [password, setPassword] = useState('');

  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    async function fetchData() {
      const tokenResponse = await AuthApi.login(login, password);
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
        <form onSubmit={handleSubmit} className={Style.form}>
          <Input label="Логин" required value={login} onChange={setLogin} />
          <Input label="Пароль" required value={password} onChange={setPassword} />
          <button className={Style.loginBtn} type="submit">
            Войти
          </button>
        </form>
      </div>
      <div className={Style.popUpNotification}>
        <ListErrorPopUp messages={errorMessage} />
      </div>
    </>
  );
};

export default AuthPage;

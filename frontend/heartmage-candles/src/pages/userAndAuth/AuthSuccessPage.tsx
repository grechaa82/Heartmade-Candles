import { FC, useState, useEffect } from 'react';
import { useNavigate, Link } from 'react-router-dom';

import { useLogoutQuery } from '../../hooks/userAndAuth/useAuthQueries';

import Style from './AuthSuccessPage.module.css';

const AuthSuccessPage: FC = () => {
  const { logout } = useLogoutQuery();
  const [timeLeft, setTimeLeft] = useState(5);

  const navigate = useNavigate();

  useEffect(() => {
    const redirectTimeout = setTimeout(() => {
      navigate(`/`);
    }, timeLeft * 1000);

    const interval = setInterval(() => {
      setTimeLeft((prevCount) => prevCount - 1);
    }, 1000);

    return () => {
      clearTimeout(redirectTimeout);
      clearInterval(interval);
    };
  }, [timeLeft, history]);

  const onSubmit = () => {
    async function fetchData() {
      logout();
    }

    fetchData();
  };

  return (
    <div className={Style.container}>
      <div className={Style.block}>
        <h3>Вы успешно авторизовались!</h3>
        <p>На главную через {timeLeft} секунд</p>
      </div>
      <div className={Style.blockBtn}>
        <Link className={Style.forAdminBtn} to={'/admin'}>
          Для администраторов
        </Link>
        <button className={Style.forLogoutBtn} onClick={onSubmit}>
          Выйти из профиля
        </button>
      </div>
    </div>
  );
};

export default AuthSuccessPage;

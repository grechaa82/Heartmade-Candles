import { FC, useState, useEffect } from 'react';
import { useNavigate, Link } from 'react-router-dom';

import Style from './SuccessfulAuthPage.module.css';

const AuthSuccessPage: FC = () => {
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

  return (
    <div className={Style.container}>
      <div className={Style.block}>
        <h3>Вы успешно авторизовались!</h3>
        <p>На главную через {timeLeft} секунд</p>
      </div>
      <Link className={Style.forAdminBtn} to={'/admin'}>
        Для администраторов
      </Link>
    </div>
  );
};

export default AuthSuccessPage;

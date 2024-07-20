import { FC, useContext, useEffect, useState } from 'react';
import { NavLink, useMatch, useNavigate } from 'react-router-dom';

import { AuthHelper } from '../../helpers/AuthHelper';
import { AuthContext } from '../../context/AuthContext';
import { AuthApi } from '../../services/AuthApi';

import Style from './Navbar.module.css';

const Navbar: FC = () => {
  const { isAuth } = useContext(AuthContext);
  const isActive = useMatch({
    path: '/',
    end: true,
  });
  const navigate = useNavigate();
  const navLinkClassName = ({ isActive }: { isActive: boolean }) =>
    `${Style.navbarItem} ${isActive ? Style.active : ''}`;

  const [position, setPosition] = useState(window.pageYOffset);
  const [visible, setVisible] = useState(true);

  useEffect(() => {
    const handleScroll = () => {
      let moving = window.pageYOffset;

      setVisible(position > moving);
      setPosition(moving);
    };
    window.addEventListener('scroll', handleScroll);
    return () => {
      window.removeEventListener('scroll', handleScroll);
    };
  });

  const onSubmit = () => {
    async function fetchData() {
      const tokenResponse = await AuthApi.logout();
      if (tokenResponse.data === null && !tokenResponse.error) {
        AuthHelper.removeToken();
        navigate('/');
      }
    }

    fetchData();
  };

  return (
    <nav
      className={
        visible ? `${Style.navbar} ${Style.scrolledNavbar}` : Style.navbar
      }
    >
      <div className={Style.navBlock}>
        <NavLink className={navLinkClassName} to="/">
          Статистика
        </NavLink>
        <NavLink className={navLinkClassName} to="/admin/orders">
          Заказы
        </NavLink>
        <NavLink className={navLinkClassName} to="/admin/candles">
          Свечи
        </NavLink>
        <NavLink className={navLinkClassName} to="/admin/decors">
          Декоры
        </NavLink>
        <NavLink className={navLinkClassName} to="/admin/layerColors">
          Слои
        </NavLink>
        <NavLink className={navLinkClassName} to="/admin/smells">
          Запахи
        </NavLink>
        <NavLink className={navLinkClassName} to="/admin/wicks">
          Фитили
        </NavLink>
        <NavLink className={navLinkClassName} to="/admin/bot">
          Бот
        </NavLink>
        <div className={Style.divider}></div>
        <div className={Style.authBlock}>
          {isAuth ? (
            <button
              className={`${Style.navbarItem} ${Style.navLogoutBtn}`}
              onClick={onSubmit}
            >
              Выйти
            </button>
          ) : (
            <NavLink
              className={`${Style.navbarItem} ${Style.navLinkAuth}`}
              to="/auth"
            >
              Войти
            </NavLink>
          )}
        </div>
      </div>
    </nav>
  );
};

export default Navbar;

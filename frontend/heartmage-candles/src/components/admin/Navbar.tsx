import { FC, useContext } from 'react';
import { NavLink, useMatch, useNavigate } from 'react-router-dom';

import { AuthApi } from '../../services/AuthApi';

import Style from './Navbar.module.css';
import { AuthHelper } from '../../helpers/AuthHelper';
import { AuthContext } from '../../context/AuthContext';

const Navbar: FC = () => {
  const { isAuth } = useContext(AuthContext);
  const isActive = useMatch({
    path: '/',
    end: true,
  });
  const navigate = useNavigate();
  const navLinkClassName = ({ isActive }: { isActive: boolean }) =>
    `${Style.navbarItem} ${isActive ? Style.active : ''}`;

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
    <nav className={Style.navbar}>
      <div className={Style.navBlock}>
        <NavLink className={navLinkClassName} to="/">
          Статистика
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
      </div>
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
    </nav>
  );
};

export default Navbar;

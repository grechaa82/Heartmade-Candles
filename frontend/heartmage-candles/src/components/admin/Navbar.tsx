import { FC } from 'react';
import { NavLink, useMatch } from 'react-router-dom';

import Style from './Navbar.module.css';

const Navbar: FC = () => {
  const isActive = useMatch({
    path: '/',
    end: true,
  });

  const navLinkClassName = ({ isActive }: { isActive: boolean }) =>
    `${Style.navbarItem} ${isActive ? Style.active : ''}`;

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
      <NavLink
        className={`${Style.navbarItem} ${Style.navLinkAuth}`}
        to="/auth"
      >
        Войти
      </NavLink>
    </nav>
  );
};

export default Navbar;

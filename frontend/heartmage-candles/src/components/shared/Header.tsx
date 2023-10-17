import { FC } from 'react';
import { NavLink, Link } from 'react-router-dom';

import Logo from '../../UI/Logo';

import Style from './Header.module.css';

const Header: FC = () => {
  return (
    <header className={Style.header}>
      <nav className={Style.navbar}>
        <NavLink to="/">
          <Logo width={103} height={50} color="#222" />
        </NavLink>
        <NavLink
          className={`${Style.navLinkClassName} ${Style.navLinkConstructor}`}
          to="/constructor"
        >
          Конструктор
        </NavLink>
        {/* <NavLink className={`${Style.navLinkClassName} ${Style.navLinkCatalog}`} to="/">
          Каталог
        </NavLink> */}
        <NavLink className={Style.navLinkClassName} to="/aboutUs">
          О нас
        </NavLink>
        <NavLink className={Style.navLinkClassName} to="/contact">
          Контакты
        </NavLink>
        <NavLink className={Style.navLinkClassName} to="/review">
          Отзывы
        </NavLink>
      </nav>
      <div className={Style.rightPart}>
        <p>Мы в разработке v0.1.0</p>
        <Link to={'/help'}>Как нам помочь?</Link>
      </div>
    </header>
  );
};

export default Header;

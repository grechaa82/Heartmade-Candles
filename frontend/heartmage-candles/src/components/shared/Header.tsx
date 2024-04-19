import { FC, useState, useEffect } from 'react';
import { NavLink, Link } from 'react-router-dom';

import Logo from '../../UI/Logo';
import LogoSmall from '../../UI/LogoSmall';
import IconMenuLarge from '../../UI/IconMenuLarge';

import Style from './Header.module.css';

const Header: FC = () => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const toggleMenu = () => {
    setIsMenuOpen(!isMenuOpen);
  };
  const [position, setPosition] = useState(window.pageYOffset);
  const [visible, setVisible] = useState(true);

  useEffect(() => {
    const handleScroll = () => {
      let moving = window.pageYOffset;

      setVisible(position > moving);
      setPosition(moving);
      setIsMenuOpen(false);
    };
    window.addEventListener('scroll', handleScroll);
    return () => {
      window.removeEventListener('scroll', handleScroll);
    };
  });

  return (
    <header
      className={visible ? `${Style.header} ${Style.visible}` : Style.header}
    >
      <div className={Style.burgerMenu}>
        <NavLink className={Style.altLogo} to="/">
          <LogoSmall />
        </NavLink>
        <button className={Style.burgerMenuIcon} onClick={toggleMenu}>
          <IconMenuLarge />
        </button>
      </div>
      <div className={`${Style.menu} ${isMenuOpen ? Style.open : ''}`}>
        <nav className={Style.navbar}>
          <NavLink className={Style.mainLogo} onClick={toggleMenu} to="/">
            <Logo width={103} height={50} color="#222" />
          </NavLink>
          <NavLink
            className={`${Style.navLinkClassName} ${Style.navLinkConstructor}`}
            onClick={toggleMenu}
            to="/constructor"
          >
            Конструктор
          </NavLink>
          {/* <NavLink className={`${Style.navLinkClassName} ${Style.navLinkCatalog}`} to="/">
          Каталог
        </NavLink> */}
          <NavLink
            className={Style.navLinkClassName}
            onClick={toggleMenu}
            to="/aboutUs"
          >
            О нас
          </NavLink>
          <NavLink
            className={Style.navLinkClassName}
            onClick={toggleMenu}
            to="/contact"
          >
            Контакты
          </NavLink>
          <NavLink
            className={Style.navLinkClassName}
            onClick={toggleMenu}
            to="/review"
          >
            Отзывы
          </NavLink>
        </nav>
        <div className={Style.rightPart}>
          <Link className={Style.helpLink} onClick={toggleMenu} to={'/help'}>
            Помочь нам [ ! ]
          </Link>
        </div>
      </div>
    </header>
  );
};

export default Header;

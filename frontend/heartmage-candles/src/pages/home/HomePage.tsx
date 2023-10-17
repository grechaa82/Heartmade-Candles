import { FC } from 'react';
import { Link } from 'react-router-dom';

import BannerImage1 from '../../assets/banner-image-1.jpg';
import BannerImage2 from '../../assets/banner-image-2.jpg';
import BannerImage3 from '../../assets/banner-image-3.jpg';

import Style from './HomePage.module.css';

const HomePage: FC = () => {
  return (
    <>
      <div className={Style.container}>
        <h1 className={Style.title}>Создай свою свечу за 5 минут</h1>
        <div className={Style.mainBanner}>
          <div className={Style.banner}>
            <div className={Style.image}>
              <img src={BannerImage1} alt="Выбор из разных свечей" />
            </div>
            <h3>Выбирайте свечи</h3>
          </div>
          <div className={Style.banner}>
            <div className={Style.image}>
              <img src={BannerImage2} alt="Создание свечи" />
            </div>
            <h3>Создавайте уникальный дизайн</h3>
          </div>
          <div className={Style.banner}>
            <div className={Style.image}>
              <img src={BannerImage3} alt="Свечи в коробке" />
            </div>
            <h3>Получайте свои свечи</h3>
          </div>
        </div>
      </div>
      <div className={Style.btnBlock}>
        <Link className={Style.btnLink} to="/constructor">
          <button className={Style.btnConstructor}>Создать в конструкторе</button>
        </Link>
        {/* <button className={Style.btnCatalog}></button> */}
      </div>
    </>
  );
};

export default HomePage;

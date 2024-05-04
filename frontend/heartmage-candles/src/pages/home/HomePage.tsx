import { FC } from 'react';
import { Link } from 'react-router-dom';

import BannerImage1 from '../../assets/banner-image-1.jpg';
import BannerImage2 from '../../assets/banner-image-2.jpg';
import BannerImage3 from '../../assets/banner-image-3.jpg';
import CustomImage from '../../components/shared/Image';

import Style from './HomePage.module.css';

const HomePage: FC = () => {
  return (
    <>
      <div className={Style.container}>
        <h1 className={Style.title}>Создай свою свечу за 5 минут</h1>
        <div className={Style.mainBanner}>
          <div className={Style.banner}>
            <div className={Style.imageBlock}>
              <CustomImage
                name={BannerImage1}
                alt="Выбор из разных свечей"
                src={BannerImage1}
                className={Style.rectangularImage}
              />
            </div>
            <h3>Выбирайте свечи</h3>
          </div>
          <div className={Style.banner}>
            <div className={Style.imageBlock}>
              <CustomImage
                name={BannerImage2}
                alt="Создание свечи"
                src={BannerImage2}
                className={Style.rectangularImage}
              />
            </div>
            <h3>Создавайте уникальный дизайн</h3>
          </div>
          <div className={Style.banner}>
            <div className={Style.imageBlock}>
              <CustomImage
                name={BannerImage3}
                alt="Свечи в коробке"
                src={BannerImage3}
                className={Style.rectangularImage}
              />
            </div>
            <h3>Получайте свои свечи</h3>
          </div>
        </div>
      </div>
      <div className={Style.btnBlock}>
        <Link className={Style.btnLink} to="/constructor">
          <button className={Style.btnConstructor}>
            Создать в конструкторе
          </button>
        </Link>
        {/* <button className={Style.btnCatalog}></button> */}
      </div>
    </>
  );
};

export default HomePage;

import { FC } from 'react';
import { Link } from 'react-router-dom';

import Picture, { SourceSettings } from '../../components/shared/Picture';

import Style from './HomePage.module.css';

const HomePage: FC = () => {
  const sourceSettings: SourceSettings[] = [
    {
      size: 'small',
      media: '(max-width: 200px)',
    },
    {
      size: 'medium',
      media: '(max-width: 630px)',
    },
    {
      size: 'large',
      media: '(max-width: 768px)',
    },
    {
      size: 'medium',
      media: '(max-width: 1896px)',
    },
    {
      size: 'large',
      media: '(min-width: 1897px)',
    },
  ];

  return (
    <>
      <div className={Style.container}>
        <h1 className={Style.title}>Создай свою свечу за 5 минут</h1>
        <div className={Style.mainBanner}>
          <div className={Style.banner}>
            <div className={Style.imageBlock}>
              <Picture
                name="banner-image-1.jpg"
                alt="Выбор из разных свечей"
                className={Style.rectangularImage}
                sourceSettings={sourceSettings}
              />
            </div>
            <h3>Выбирайте свечи</h3>
          </div>
          <div className={Style.banner}>
            <div className={Style.imageBlock}>
              <Picture
                name="banner-image-2.jpg"
                alt="Создание свечи"
                className={Style.rectangularImage}
                sourceSettings={sourceSettings}
              />
            </div>
            <h3>Создавайте уникальный дизайн</h3>
          </div>
          <div className={Style.banner}>
            <div className={Style.imageBlock}>
              <Picture
                name="banner-image-3.jpg"
                alt="Свечи в коробке"
                className={Style.rectangularImage}
                sourceSettings={sourceSettings}
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

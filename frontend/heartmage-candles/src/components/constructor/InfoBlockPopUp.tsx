import { FC, useState, useEffect } from 'react';

import Style from './InfoBlockPopUp.module.css';

export interface InfoBlockPopUpProps {
  title: string;
  description: string;
  x: number;
  y: number;
}

const InfoBlockPopUp: FC<InfoBlockPopUpProps> = ({
  title,
  description,
  x,
  y,
}) => {
  const [positionX, setPositionX] = useState(x);
  const [positionY, setPositionY] = useState(y);
  const [isVisible, setIsVisible] = useState(false);
  const [screenWidth, setScreenWidth] = useState(window.innerWidth);

  useEffect(() => {
    function handleResize() {
      setScreenWidth(window.innerWidth);
    }

    window.addEventListener('resize', handleResize);

    return () => {
      window.removeEventListener('resize', handleResize);
    };
  }, []);

  useEffect(() => {
    let newX = x;
    let newY = y;

    const popupWidth = 340;

    const spaceFromEdge = 10;

    if (x + popupWidth > screenWidth) {
      newX = screenWidth - popupWidth - spaceFromEdge;
    }

    setPositionX(newX);
    setPositionY(newY);
    setIsVisible(true);
  }, [x, y, screenWidth]);

  return (
    <>
      {isVisible ? (
        <div
          className={Style.descriptionMenu}
          style={{
            position: 'fixed',
            top: positionY,
            left: positionX,
          }}
        >
          <p className={Style.descriptionMenuTitle}>{title}</p>
          <p className={Style.descriptionMenuDescription}>{description}</p>
        </div>
      ) : (
        <div></div>
      )}
    </>
  );
};

export default InfoBlockPopUp;

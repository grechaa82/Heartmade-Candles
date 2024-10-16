import { FC, useEffect, useState } from 'react';

import IconArrowLeftLarge from '../../UI/IconArrowLeftLarge';

import Style from './ScrollToTopButton.module.css';

export interface ScrollToTopButtonProps {}

const ScrollToTopButton: FC<ScrollToTopButtonProps> = () => {
  const [isVisible, setIsVisible] = useState(false);

  const toggleVisibility = () => {
    if (window.scrollY > 300) {
      setIsVisible(true);
    } else {
      setIsVisible(false);
    }
  };

  const scrollToTop = () => {
    window.scrollTo({
      top: 0,
      behavior: 'smooth',
    });
  };

  useEffect(() => {
    window.addEventListener('scroll', toggleVisibility);
    return () => {
      window.removeEventListener('scroll', toggleVisibility);
    };
  }, []);

  return (
    isVisible && (
      <button className={Style.toTopBtn} onClick={scrollToTop}>
        <div className={Style.icon}>
          <IconArrowLeftLarge />
        </div>
      </button>
    )
  );
};

export default ScrollToTopButton;

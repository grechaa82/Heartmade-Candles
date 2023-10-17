import { FC } from 'react';

import Style from './HelpPage.module.css';

const HelpPage: FC = () => {
  return (
    <div className={Style.block}>
      <h3>Будем благодарны за любую помощь</h3>
      <p>Свяжитесь с нами или опишите проблему в issues</p>
      <div className={Style.btnBlock}>
        <div className={`${Style.btn} ${Style.github}`}>
          <a
            href="https://github.com/grechaa82/Heartmade-Candles/issues"
            target="_blank"
            rel="noopener noreferrer"
          >
            Github issues
          </a>
        </div>
        <div className={Style.btn}>
          <a href="https://t.me/grechaa82" target="_blank" rel="noopener noreferrer">
            Telegram
          </a>
        </div>
      </div>
    </div>
  );
};

export default HelpPage;

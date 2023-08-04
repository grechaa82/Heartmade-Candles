import { FC } from 'react';

import Style from './CornerTag.module.css';

type typeCornerTag = 'coutner' | 'price';

export interface CornerTagProps {
  number: number;
  type?: typeCornerTag;
  isLeft?: boolean;
}

const CornerTag: FC<CornerTagProps> = ({ number, type, isLeft = true }) => {
  const cornerTagStyle = isLeft ? Style.leftCornerTag : Style.rightCornerTag;
  const cornerTagContent = type === 'coutner' ? '' : type === 'price' ? 'р' : null;

  return (
    <div className={`${Style.cornerTag} ${cornerTagStyle}`}>
      <span>{number}</span>
      {cornerTagContent && <span> {cornerTagContent}</span>}
    </div>
  );
};

export default CornerTag;

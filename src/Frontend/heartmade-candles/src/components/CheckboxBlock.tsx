import React, { FC } from 'react';
import Checkbox from './Checkbox';

import Style from './CheckboxBlock.module.css';

interface CheckboxBlockProps {
  text: string;
  color?: string;
  checked: boolean;
}

const CheckboxBlock: React.FC<CheckboxBlockProps> = ({ text, color = '#222', checked }) => {
  const CheckboxBlockStyle = {
    color: color,
  };

  return (
    <div className={Style.checkboxBlock}>
      <p style={CheckboxBlockStyle}>{text}</p>
      <Checkbox checked={checked} />
    </div>
  );
};

export default CheckboxBlock;

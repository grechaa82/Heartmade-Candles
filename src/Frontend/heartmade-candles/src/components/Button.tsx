import React, { FC } from 'react';
import IconPlusLarge from '../UI/IconPlusLarge';
import Style from './Button.module.css';

interface ButtonProps {
  text?: string;
  color?: string;
  height?: number;
  width?: number;
  // TODO: add props: Icon, SizeByContent
}

const Button: React.FC<ButtonProps> = ({
  text = 'Add',
  color = '#000',
  height = 56,
  width = 320,
}) => {
  const buttonStyle = {
    color: color,
    height: `${height - 2}px`,
    width: `${width}px`,
  };

  return (
    <button className={Style.button}>
      <div style={buttonStyle}>
        <IconPlusLarge color={color} />
        <p>{text}</p>
      </div>
    </button>
  );
};

export default Button;

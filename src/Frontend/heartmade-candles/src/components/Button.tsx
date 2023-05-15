import React, { FC } from 'react';
import Style from './Button.module.css';

export interface ButtonProps {
  text: string;
  color?: string;
  height?: number;
  width?: number;
  onClick: () => void;
}

const Button: React.FC<ButtonProps> = ({ text, color = '#000', height, width, onClick }) => {
  return (
    <button
      className={Style.button}
      style={{
        color,
        ...(height && { height: `${height - 4}px` }),
        ...(width && { width: `${width}px` }),
      }}
      onClick={onClick}
    >
      <p>{text}</p>
    </button>
  );
};

export default Button;

import { FC } from 'react';
import Style from './Button.module.css';

export interface ButtonProps {
  text: string;
  color?: string;
  height?: number;
  width?: number;
}

export interface ButtonDefaultProps extends ButtonProps {
  onClick: () => void;
}

const Button: FC<ButtonDefaultProps> = ({ text, color = '#000', height, width, onClick }) => {
  return (
    <button
      className={Style.button}
      style={{
        color,
        ...(height && { height: `${height - 4}px` }),
        ...(width && { width: `${width}px` }),
      }}
      onClick={onClick}
      type="button"
    >
      <p>{text}</p>
    </button>
  );
};

export default Button;

import { FC } from 'react';
import Style from './Button.module.css';

export interface ButtonProps {
  text: string;
  color?: string;
  height?: number;
  width?: number;
  size?: 'xl' | 'l' | 'm' | 's';
}

export interface ButtonDefaultProps extends ButtonProps {
  className?: string;
  onClick: () => void;
}

const Button: FC<ButtonDefaultProps> = ({
  text,
  color,
  height,
  width,
  size = 'l',
  className,
  onClick,
}) => {
  const buttonSizeClass = `buttonSize_${size}`;
  const textSizeClass = `textSize_${size}`;

  return (
    <button
      className={`${Style.button} ${className ? className : ''} ${
        Style[buttonSizeClass]
      }`}
      style={{
        color,
        ...(height && { height: `${height - 4}px` }),
        ...(width && { width: `${width}px` }),
      }}
      onClick={onClick}
      type="button"
    >
      <p className={Style[textSizeClass]}>{text}</p>
    </button>
  );
};

export default Button;

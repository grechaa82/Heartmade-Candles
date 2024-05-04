import { FC } from 'react';
import Style from './Button.module.css';

export interface ButtonProps {
  text: string;
  color?: string;
  height?: number;
  width?: number;
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
  className,
  onClick,
}) => {
  return (
    <button
      className={`${Style.button} ${className ? className : ''}`}
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

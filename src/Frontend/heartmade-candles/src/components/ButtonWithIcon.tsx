import React, { FC } from 'react';
import { IconProps } from '../UI/IconProps';
import Button, { ButtonProps } from './Button';
import Style from './ButtonWithIcon.module.css';
import StyleButton from './Button.module.css';

interface ButtonWithIconProps extends ButtonProps {
  icon: React.FC<IconProps>;
}

const ButtonWithIcon: React.FC<ButtonWithIconProps> = ({ icon: Icon, ...buttonProps }) => {
  const buttonStyle = {
    color: buttonProps.color,
    ...(buttonProps.height && { height: `${buttonProps.height - 4}px` }),
    ...(buttonProps.width && { width: `${buttonProps.width}px` }),
  };

  return (
    <button
      className={`${StyleButton.button} ${Style.buttonButtonWithIcon}`}
      onClick={buttonProps.onClick}
      style={buttonStyle}
    >
      <Icon color={buttonProps.color} />
      <p>{buttonProps.text}</p>
    </button>
  );
};

export default ButtonWithIcon;

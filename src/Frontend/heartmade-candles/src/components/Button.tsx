import React, { FC } from 'react';
import { IconProps } from '../UI/IconProps';
import IconChevronDownLarge from '../UI/IconChevronDownLarge';
import Style from './Button.module.css';

interface ButtonProps {
  text?: string;
  color?: string;
  height?: number;
  width?: number;
  type?: 'Normal' | 'Dropdown' | 'WithIcon';
  icon?: React.FC<IconProps>;
}

const Button: React.FC<ButtonProps> = ({
  type = 'Normal',
  text = 'Add',
  color = '#000',
  height,
  width,
  icon,
}) => {
  if (type === 'WithIcon' && !icon) {
    throw new Error('prop "icon" is required for type "WithIcon"');
  }

  return (
    <button
      className={Style.button + ' ' + getButtonClassName(type)}
      style={{
        color,
        ...(height && { height: `${height - 4}px` }),
        ...(width && { width: `${width}px` }),
      }}
    >
      {type === 'WithIcon' && icon ? React.createElement(icon, { color: color }) : null}
      <p>{text}</p>
      {type === 'Dropdown' ? (
        <div className={Style.dropdownIcon}>
          <span></span>
          <IconChevronDownLarge color="#aaa" />
        </div>
      ) : null}
    </button>
  );
};

function getButtonClassName(type: ButtonProps['type']): string {
  if (type === 'Dropdown') {
    return Style.buttonDropdown;
  }

  if (type === 'WithIcon') {
    return Style.buttonWithIcon;
  }

  return Style.buttonNormal;
}

export default Button;

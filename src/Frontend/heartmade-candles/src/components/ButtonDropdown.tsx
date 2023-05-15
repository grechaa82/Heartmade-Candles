import React, { FC, useState } from 'react';
import Button, { ButtonProps } from './Button';
import IconChevronDownLarge from '../UI/IconChevronDownLarge';
import Style from './ButtonDropdown.module.css';
import StyleButton from './Button.module.css';

interface ButtonDropdownProps extends ButtonProps {
  options: string[];
}

const ButtonDropdown: React.FC<ButtonDropdownProps> = ({ options, ...buttonProps }) => {
  const [isOpen, setIsOpen] = useState(false);

  const buttonStyle = {
    color: buttonProps.color,
    ...(buttonProps.height && { height: `${buttonProps.height - 4}px` }),
    ...(buttonProps.width && { width: `${buttonProps.width}px` }),
  };

  return (
    <div>
      <button
        className={`${StyleButton.button} ${Style.buttonDropdown}`}
        onClick={buttonProps.onClick}
        style={buttonStyle}
      >
        <p>{buttonProps.text}</p>
        <div className={Style.dropdownIcon}>
          <IconChevronDownLarge color="#aaa" />
        </div>
      </button>
      {isOpen && (
        <div style={{ position: 'absolute', top: '100%', left: 0 }}>
          {options.map((option) => (
            <Button text={option} onClick={() => console.log()} />
          ))}
        </div>
      )}
    </div>
  );
};

export default ButtonDropdown;

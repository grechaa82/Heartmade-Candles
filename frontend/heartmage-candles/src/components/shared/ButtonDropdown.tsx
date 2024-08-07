import { useState } from 'react';

import { ButtonProps } from './Button';
import IconChevronDownLarge from '../../UI/IconChevronDownLarge';

import StyleButton from './Button.module.css';
import Style from './ButtonDropdown.module.css';

export interface optionData {
  id: string;
  title: string;
}

interface ButtonDropdownProps<T extends optionData> extends ButtonProps {
  options: T[];
  selected: T;
  onChange: (value: T) => void;
}

const ButtonDropdown = <T extends optionData>({
  text,
  color = '#000',
  height,
  width,
  options,
  selected,
  size = 'l',
  onChange,
}: ButtonDropdownProps<T>) => {
  const [isOpen, setIsOpen] = useState(false);

  const buttonStyle = {
    color: color,
    ...(height && { height: `${height - 4}px` }),
    ...(width && { width: `${width}px` }),
  };

  const buttonSizeClass = `buttonSize_${size}`;
  const textSizeClass = `textSize_${size}`;

  const handleOptionClick = (option: T) => {
    onChange(option);
    setIsOpen(false);
  };

  return (
    <div className={Style.dropdownBlock} onClick={() => setIsOpen(!isOpen)}>
      <button
        style={buttonStyle}
        className={`${StyleButton.button} ${Style.buttonDropdown} ${
          isOpen ? Style.buttonDropdownIsOpen : ''
        } ${StyleButton[buttonSizeClass]}`}
        type="button"
      >
        <p className={StyleButton[textSizeClass]}>{selected.title}</p>
        <div className={Style.dropdownIcon}>
          <IconChevronDownLarge color="#aaa" />
        </div>
      </button>
      {isOpen && (
        <div className={Style.dropdownMenu}>
          <ul>
            {options.map((option) => (
              <li
                key={option.id}
                onClick={() => handleOptionClick(option)}
                className={StyleButton[textSizeClass]}
              >
                {option.title}
              </li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
};

export default ButtonDropdown;

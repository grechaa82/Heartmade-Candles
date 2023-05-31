import { ChangeEvent, FC, useState } from 'react';
import { ButtonProps } from './Button';
import IconChevronDownLarge from '../UI/IconChevronDownLarge';

import StyleButton from './Button.module.css';
import Style from './ButtonDropdown.module.css';

export interface optionData {
  id: string;
  title: string;
}

export interface ButtonDropdownProps extends ButtonProps {
  options: optionData[];
  onClick: (id: string) => void;
}

const ButtonDropdown: FC<ButtonDropdownProps> = ({
  options,
  onClick,
  text,
  color = '#000',
  height,
  width,
}) => {
  const [isOpen, setIsOpen] = useState(false);
  const [selectedOption, setSelectedOption] = useState<null | optionData>(null);

  const buttonStyle = {
    color: color,
    ...(height && { height: `${height - 4}px` }),
    ...(width && { width: `${width}px` }),
  };

  const handleOptionClick = (optionData: optionData) => {
    setSelectedOption(optionData);
    setIsOpen(false);
    onClick(optionData.id);
  };

  return (
    <div className={Style.dropdownBlock} onClick={() => setIsOpen(!isOpen)}>
      <button
        style={buttonStyle}
        className={`${StyleButton.button} ${Style.buttonDropdown}`}
        type="button"
      >
        <p>{selectedOption ? selectedOption.title : text}</p>
        <div className={Style.dropdownIcon}>
          <IconChevronDownLarge color="#aaa" />
        </div>
      </button>
      {isOpen && (
        <div className={Style.dropdownMenu}>
          <ul>
            {options.map((option) => (
              <li key={option.id} onClick={() => handleOptionClick(option)}>
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

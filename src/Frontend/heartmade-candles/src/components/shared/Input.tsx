import { FC, ChangeEvent } from 'react';

import IconAlertCircleLarge from '../../UI/IconAlertCircleLarge';

import Style from './Input.module.css';

export interface InputProps {
  label: string;
  required?: boolean;
  value: string;
  onChange: (value: string) => void;
  validation?: (value: string) => boolean;
  type?: string;
  pattern?: string;
  placeholder?: string;
}

const Input: FC<InputProps> = ({
  label,
  required = false,
  value,
  onChange,
  validation,
  type = 'text',
  pattern,
  placeholder,
}) => {
  const handleInputChange = (event: ChangeEvent<HTMLInputElement>) => {
    onChange(event.target.value);
  };

  const isValid = validation ? validation(value) : true;

  return (
    <div className={`${Style.input} ${!isValid && value ? Style.invalidInput : ''}`}>
      <label>
        {label}
        {required && !value && <span> *</span>}
      </label>
      <input
        type={type}
        value={value}
        onChange={handleInputChange}
        pattern={pattern}
        placeholder={placeholder}
      />
      <div className={Style.validationBlock}>
        {!isValid && value && (
          <span>
            <IconAlertCircleLarge color="#eb5757" />
          </span>
        )}
        {/* {!isValid && <span>Invalid input</span>} */}
      </div>
    </div>
  );
};

export default Input;

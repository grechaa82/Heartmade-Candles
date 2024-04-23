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
  errorMessage?: string;
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
  errorMessage,
}) => {
  const handleInputChange = (event: ChangeEvent<HTMLInputElement>) => {
    onChange(event.target.value);
  };

  const isValid = validation ? validation(value) : true;

  return (
    <>
      <div className={Style.inputWrapper}>
        <label className={Style.label}>
          {label} {required && !value && <span> *</span>}
        </label>
        <input
          type={type}
          value={value}
          onChange={handleInputChange}
          className={Style.input}
          pattern={pattern}
          placeholder={placeholder}
          required
        />
        {!isValid && <p className={Style.validationError}>{errorMessage}</p>}
      </div>
    </>
  );
};

export default Input;

import { FC } from 'react';
import { useFormContext } from 'react-hook-form';

import Style from './Input.module.css';

interface InputProps {
  name: string;
  error?: string;
  label: string;
}

const Input: FC<InputProps> = ({ name, error, label }) => {
  const methods = useFormContext();

  return (
    <div className={Style.inputWrapper}>
      <label className={Style.label}>{label}</label>
      <input {...methods.register(name)} className={Style.input} />
      {error && <p className={Style.validationError}>{error}</p>}
    </div>
  );
};

export default Input;

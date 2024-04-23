import { InputHTMLAttributes, forwardRef } from 'react';

import Style from './Input.module.css';

export type InputV2Props = InputHTMLAttributes<HTMLInputElement> & {
  label: string;
  name: string;
  error?: string | undefined;
};

const InputV2 = forwardRef<HTMLInputElement, InputV2Props>(
  ({ name, label, error, ...rest }, ref) => {
    return (
      <div className={Style.inputWrapper}>
        <label className={Style.label}>{label}</label>
        <input
          id={name}
          className={Style.input}
          name={name}
          ref={ref}
          {...rest}
        />
        {error && <p className={Style.validationError}>{error}</p>}
      </div>
    );
  },
);

export default InputV2;

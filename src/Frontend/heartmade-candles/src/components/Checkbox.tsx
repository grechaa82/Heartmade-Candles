import React, { FC } from 'react';
import { useState } from 'react';
import IconCheckLarge from '../UI/IconCheckLarge';
import Style from './Checkbox.module.css';

interface CheckboxProps {}

const Checkbox: React.FC<CheckboxProps> = () => {
  return (
    <label className={Style.checkbox}>
      <input type="checkbox" />
      <div className={Style.checkboxIcon}>
        <IconCheckLarge color="#fff" />
      </div>
    </label>
  );
};

export default Checkbox;

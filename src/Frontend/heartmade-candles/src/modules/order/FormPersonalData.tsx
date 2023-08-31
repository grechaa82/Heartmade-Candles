import { FC, useState } from 'react';

import Input from '../../components/order/Input';

import Style from './FormPersonalData.module.css';

export interface FormPersonalDataProps {}

const FormPersonalData: FC<FormPersonalDataProps> = ({}) => {
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [email, setEmail] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');

  const validateFirstNameAndLastName = (value: string) => {
    const regex = /^[a-zA-Zа-яА-Я]+$/;
    return regex.test(value);
  };

  const validateEmail = (value: string) => {
    return /^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$/.test(value);
  };

  const validatePhoneNumber = (value: string) => {
    return /^((8|\+7|7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?\d{3}[\- ]?\d{2}[\- ]?\d{2}?$/.test(value);
  };

  return (
    <div>
      <div className={Style.title}>Личные данные</div>
      <form action="" className={Style.form}>
        <Input
          label="Имя"
          required
          value={firstName}
          onChange={setFirstName}
          validation={validateFirstNameAndLastName}
        />
        <Input
          label="Фамилия"
          required
          value={lastName}
          onChange={setLastName}
          validation={validateFirstNameAndLastName}
        />
        <Input
          label="Элекронная почта"
          value={email}
          onChange={setEmail}
          validation={validateEmail}
          type="email"
        />
        <Input
          label="Номер телефона"
          required
          value={phoneNumber}
          onChange={setPhoneNumber}
          validation={validatePhoneNumber}
          type="tel"
        />
      </form>
    </div>
  );
};

export default FormPersonalData;

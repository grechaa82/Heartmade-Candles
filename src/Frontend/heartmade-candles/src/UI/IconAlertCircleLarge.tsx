import React from 'react';
import { IconProps } from './IconProps';

const IconAlertCircleLarge: React.FC<IconProps> = ({ color = '#000' }) => {
  return (
    <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
      <path
        fillRule="evenodd"
        clipRule="evenodd"
        d="M1 12C1 5.92472 5.92472 1 12 1C18.0753 1 23 5.92472 23 12C23 18.0753 18.0753 23 12 23C5.92472 23 1 18.0753 1 12ZM12 3C7.02928 3 3 7.02928 3 12C3 16.9707 7.02928 21 12 21C16.9707 21 21 16.9707 21 12C21 7.02928 16.9707 3 12 3ZM12 7C12.5523 7 13 7.44772 13 8V12C13 12.5523 12.5523 13 12 13C11.4477 13 11 12.5523 11 12V8C11 7.44772 11.4477 7 12 7ZM11 16C11 15.4477 11.4477 15 12 15H12.01C12.5623 15 13.01 15.4477 13.01 16C13.01 16.5523 12.5623 17 12.01 17H12C11.4477 17 11 16.5523 11 16Z"
        fill={color}
      />
    </svg>
  );
};

export default IconAlertCircleLarge;

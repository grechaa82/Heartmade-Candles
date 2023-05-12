import React from 'react';

interface IconChevronDownLargeProps {
  color?: string;
}

const IconChevronDownLarge: React.FC<IconChevronDownLargeProps> = ({ color = '#000' }) => {
  return (
    <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
      <path
        fill-rule="evenodd"
        clip-rule="evenodd"
        d="M5.29289 9.29607C5.68342 8.90131 6.31658 8.90131 6.70711 9.29607L12 14.5596L17.2929 9.29607C17.6834 8.90131 18.3166 8.90131 18.7071 9.29607C19.0976 9.69082 19.0976 10.3309 18.7071 10.7256L12.7071 16.7039C12.3166 17.0987 11.6834 17.0987 11.2929 16.7039L5.29289 10.7256C4.90237 10.3309 4.90237 9.69083 5.29289 9.29607Z"
        fill={color}
      />
    </svg>
  );
};

export default IconChevronDownLarge;

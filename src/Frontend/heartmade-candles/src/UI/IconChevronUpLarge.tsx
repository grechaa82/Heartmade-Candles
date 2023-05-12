import React from 'react';

interface IconChevronUpLargeProps {
  color?: string;
}

const IconChevronUpLarge: React.FC<IconChevronUpLargeProps> = ({ color = '#000' }) => {
  return (
    <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
      <path
        fill-rule="evenodd"
        clip-rule="evenodd"
        d="M6.70711 15.7039C6.31658 16.0987 5.68342 16.0987 5.29289 15.7039C4.90237 15.3092 4.90237 14.6691 5.29289 14.2744L11.2929 8.29607C11.6834 7.90131 12.3166 7.90131 12.7071 8.29607L18.7071 14.2744C19.0976 14.6691 19.0976 15.3092 18.7071 15.7039C18.3166 16.0987 17.6834 16.0987 17.2929 15.7039L12 10.4404L6.70711 15.7039Z"
        fill={color}
      />
    </svg>
  );
};

export default IconChevronUpLarge;

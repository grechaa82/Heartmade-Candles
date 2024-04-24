import { FC, ReactNode } from 'react';

import Style from './FormFeedback.module.css';

export interface FormFeedbackProps {
  title?: string;
  description?: string;
  children?: ReactNode;
}

const FormFeedback: FC<FormFeedbackProps> = ({
  title,
  description,
  children,
}) => {
  return (
    <div className={Style.formFeedback}>
      {title && <div className={Style.title}>{title}</div>}
      {description && <div className={Style.description}>{description}</div>}
      <div className={Style.form}>{children}</div>
    </div>
  );
};

export default FormFeedback;

import { ChangeEvent, FC, useState, useEffect } from 'react';
import Style from './Textarea.module.css';

export interface LimitationProps {
  limit: number;
}

interface TextareaProps {
  text: string;
  label: string;
  height?: number;
  width?: number;
  limitation?: LimitationProps;
  onChange?: (event: ChangeEvent<HTMLTextAreaElement>) => void;
}

const Textarea: FC<TextareaProps> = ({ text, label, height, width, limitation, onChange }) => {
  const [value, setValue] = useState(text);
  const [charCount, setCharCount] = useState<number>(text.length);

  useEffect(() => {
    setValue(text);
  }, [text]);

  const handleChange = (e: ChangeEvent<HTMLTextAreaElement>) => {
    const newValue = e.target.value;
    if (!limitation || newValue.length <= limitation.limit) {
      setValue(newValue);
      setCharCount(newValue.length);
      if (onChange) {
        onChange(e);
      }
    }
  };

  return (
    <div className={Style.textarea}>
      <textarea
        value={value}
        onChange={handleChange}
        style={{
          ...(height && { height: `${height}px` }),
          ...(width && { width: `${width}px` }),
        }}
      >
        {text}
      </textarea>
      <label>{label}</label>
      {limitation && (
        <p>
          {charCount} / {limitation.limit}
        </p>
      )}
    </div>
  );
};

export default Textarea;

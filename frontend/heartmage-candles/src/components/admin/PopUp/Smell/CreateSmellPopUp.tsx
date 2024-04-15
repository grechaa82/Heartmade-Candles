import { FC } from 'react';
import { useForm, SubmitHandler, Controller } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';

import { Smell } from '../../../../types/Smell';
import Textarea from '../../Textarea';
import CheckboxBlock from '../../CheckboxBlock';
import PopUp, { PopUpProps } from '../PopUp';

import Style from './CreateSmellPopUp.module.css';
import { smellSchema, SmellType } from './Smell.schema';

export interface CreateSmellPopUpProps extends PopUpProps {
  title: string;
  onSave: (smell: Smell) => void;
}

const CreateSmellPopUp: FC<CreateSmellPopUpProps> = ({
  onClose,
  title,
  onSave,
}) => {
  const {
    handleSubmit,
    formState: { isValid, errors },
    control,
  } = useForm({
    mode: 'onChange',
    resolver: yupResolver(smellSchema),
    defaultValues: {
      title: '',
      description: '',
      isActive: false,
      price: 0,
    },
  });

  const onSubmit: SubmitHandler<SmellType> = (data) => {
    const smell: Smell = {
      id: 0,
      title: data.title,
      description: data.description,
      images: [],
      isActive: data.isActive,
      price: data.price,
    };
    onSave(smell);
    onClose();
  };

  return (
    <PopUp onClose={onClose}>
      <div className={Style.container}>
        <p className={Style.title}>{title}</p>
        <form
          className={`${Style.gridContainer} ${Style.formForSmell}`}
          onSubmit={handleSubmit(onSubmit)}
        >
          <div className={`${Style.formItem} ${Style.itemTitle}`}>
            <Controller
              name="title"
              control={control}
              render={({ field }) => (
                <Textarea
                  text={field.value}
                  label="Название"
                  onChange={field.onChange}
                />
              )}
            />
            {errors?.title && (
              <p className={Style.validationError}>{errors.title.message}</p>
            )}
          </div>
          <div className={`${Style.formItem} ${Style.itemPrice}`}>
            <Controller
              name="price"
              control={control}
              render={({ field }) => (
                <Textarea
                  text={field.value.toString()}
                  label="Стоимость"
                  onChange={field.onChange}
                />
              )}
            />
            {errors?.price && (
              <p className={Style.validationError}>{errors.price.message}</p>
            )}
          </div>
          <div className={`${Style.formItem} ${Style.itemActive}`}>
            <Controller
              name="isActive"
              control={control}
              render={({ field }) => (
                <CheckboxBlock
                  text="Активна"
                  checked={field.value}
                  onChange={field.onChange}
                />
              )}
            />
            {errors?.isActive && (
              <p className={Style.validationError}>{errors.isActive.message}</p>
            )}
          </div>
          <div className={`${Style.formItem} ${Style.itemDescription}`}>
            <Controller
              name="description"
              control={control}
              render={({ field }) => (
                <Textarea
                  text={field.value}
                  label="Описание"
                  onChange={field.onChange}
                />
              )}
            />
            {errors?.description && (
              <p className={Style.validationError}>
                {errors.description.message}
              </p>
            )}
          </div>
          <button
            type="submit"
            className={`${Style.saveButton} ${
              errors.title ||
              errors.description ||
              errors.price ||
              errors.isActive
                ? Style.invalid
                : isValid
                ? Style.valid
                : Style.default
            }`}
            disabled={!isValid || Object.keys(errors).length > 0}
          >
            Сохранить
          </button>
        </form>
      </div>
    </PopUp>
  );
};

export default CreateSmellPopUp;

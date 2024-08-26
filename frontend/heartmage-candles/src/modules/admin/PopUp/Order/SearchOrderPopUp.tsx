import { FC, useState } from 'react';
import { useForm, SubmitHandler } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';

import PopUp, { PopUpProps } from '../../../../components/shared/PopUp/PopUp';
import { Order } from '../../../../typesV2/shared/Order';
import {
  getStatusString,
  getStatusStringRus,
} from '../../../../typesV2/order/OrderStatus';
import ListProductsCart from '../../../order/ListProductsCart';

import Style from './SearchOrderPopUp.module.css';

export interface SearchOrderPopUpProps extends PopUpProps {
  fetchOrderById: (orderId: string) => Promise<Order>;
}

export const searchOrderSchema = yup
  .object()
  .shape({
    searchString: yup
      .string()
      .required('Is required')
      .length(24, 'The length should be 24 characters'),
  })
  .required();

export type SearchOrderType = {
  searchString: string;
};

const SearchOrderPopUp: FC<SearchOrderPopUpProps> = ({
  onClose,
  fetchOrderById,
}) => {
  const [order, setOrder] = useState<Order>();

  const {
    register,
    handleSubmit,
    reset,
    formState: { isValid, errors },
  } = useForm({
    mode: 'onChange',
    resolver: yupResolver(searchOrderSchema),
  });

  const handleOnSubmit: SubmitHandler<SearchOrderType> = async (data) => {
    const order = await fetchOrderById(data.searchString);

    if (order) {
      setOrder(order);
    } else {
      console.log('Ничего не нашли');
    }

    reset();
  };

  const handleFormatDate = (date: Date) => {
    const day = date.getDate();
    const month = date.toLocaleString('default', { month: 'short' });
    return `${day} ${month}`;
  };

  return (
    <PopUp onClose={onClose}>
      <div className={Style.container}>
        <form onSubmit={handleSubmit(handleOnSubmit)} className={Style.form}>
          <div className={Style.inputWrapper}>
            <label className={Style.label}>Id заказа</label>
            <input className={Style.input} {...register('searchString')} />
            {errors?.searchString && (
              <p className={Style.validationError}>
                {errors.searchString.message}
              </p>
            )}
          </div>
          <button type="submit" className={`${Style.searchButton} `}>
            Найти
          </button>
        </form>
        {order && (
          <>
            <p className={Style.title}>
              <span>{order.id}</span>
              <span className={Style[getStatusString(order.status)]}>
                {getStatusStringRus(order.status)}
              </span>
            </p>
            <div className={Style.infoBlock}>
              <p className={Style.secondaryInfo}>
                Строка конфигурации <span>{order.basket.filterString}</span>
              </p>
              <p className={Style.secondaryInfo}>
                Создано{' '}
                <span>{handleFormatDate(new Date(order.createdAt))}</span>
              </p>
              <p className={Style.secondaryInfo}>
                Обновлено{' '}
                <span>{handleFormatDate(new Date(order.updatedAt))}</span>
              </p>
              <p className={Style.secondaryInfo}>
                Общее количество <span>{order.basket.totalQuantity}</span>
              </p>
              <p className={`${Style.secondaryInfo} ${Style.priceInfo}`}>
                Сумма <span>{order.basket.totalPrice}</span>
              </p>
            </div>
            <ListProductsCart products={order.basket.items} />
          </>
        )}
      </div>
    </PopUp>
  );
};

export default SearchOrderPopUp;

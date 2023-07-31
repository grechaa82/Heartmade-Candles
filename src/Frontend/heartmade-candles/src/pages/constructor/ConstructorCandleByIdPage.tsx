import { FC, useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';

import { CandleDetail } from '../../typesV2/BaseProduct';
import CandleForm from '../../modules/constructor/CandleForm';

import Style from './ConstructorCandleByIdPage.module.css';

import { ConstructorApi } from '../../services/ConstructorApi';

type CandleDetailsParams = {
  id: string;
};

const ConstructorCandleByIdPage: FC = () => {
  const { id } = useParams<CandleDetailsParams>();
  const [candleDetail, setCandleDetail] = useState<CandleDetail>();

  useEffect(() => {
    async function fetchData() {
      try {
        if (id) {
          const candleDetail = await ConstructorApi.getCandleById(id);
          setCandleDetail(candleDetail);
        }
      } catch (error) {
        console.error('Произошла ошибка при загрузке данных:', error);
      }
    }

    fetchData();
  }, [id]);

  return (
    <div className={Style.container}>
      <div className={Style.rightPanel}>
        {candleDetail && <CandleForm candleDetailData={candleDetail} />}
      </div>
    </div>
  );
};

export default ConstructorCandleByIdPage;

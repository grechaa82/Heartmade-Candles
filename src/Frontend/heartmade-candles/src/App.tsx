import React, { useState, useEffect } from 'react';
import logo from './logo.svg';
import './App.css';
import Button from './components/Button';
import ButtonDropdown from './components/ButtonDropdown';
import ButtonWithIcon from './components/ButtonWithIcon';
import Checkbox from './components/Checkbox';
import CheckboxBlock from './components/CheckboxBlock';
import Tag from './components/Tag';
import InputTag from './components/InputTag';
import Textarea from './components/Textarea';
import Icon from './UI/IconPlusLarge';
import MainInfoCandles, { CandleData } from './modules/MainInfoCandles';

function App() {
  const [candleData, setCandleData] = useState(null);

  useEffect(() => {
    fetch(`http://localhost:5000/api/admin/candles/1`)
      .then((response) => response.json())
      .then((data) => {
        setCandleData(data);
      })
      .catch((error) => {
        console.error(error);
      });
  }, []);

  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.tsx</code> and save to reload. Test text.
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
        <div className="df">
          <Button text="Добавить" onClick={() => console.log('Click')} />
          <ButtonDropdown
            options={['Option 1', 'Option 2', 'Option 3']}
            text="Dropdown кнопка"
            onClick={() => console.log('Click')}
          />
          <ButtonWithIcon text="ButtonWithIcon" icon={Icon} onClick={() => console.log('Click')} />
        </div>
        <Checkbox />
        <CheckboxBlock text="Активна" />
        <Tag id={1} text="6" />
        <InputTag />
      </header>
      <div className="formMainInfoCandles">
        {candleData ? <MainInfoCandles candleData={candleData} /> : null}
      </div>
    </div>
  );
}

export default App;

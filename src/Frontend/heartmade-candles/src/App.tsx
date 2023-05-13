import React from 'react';
import logo from './logo.svg';
import './App.css';
import Button from './components/Button';
import Checkbox from './components/Checkbox';
import CheckboxBlock from './components/CheckboxBlock';
import Tag from './components/Tag';
import InputTag from './components/InputTag';

function App() {
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
        <Button width={320} color="#2E67EA" text="Добавить" />
        <Checkbox />
        <CheckboxBlock text="Активна" />
        <Tag id={1} text="6" />
        <InputTag />
      </header>
    </div>
  );
}

export default App;

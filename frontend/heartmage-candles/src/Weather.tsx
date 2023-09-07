import { FC, useState, useEffect } from 'react';

export interface WeatherProps {}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

const Weather: FC<WeatherProps> = () => {
  const [weatherForecasts, setWeatherForecasts] = useState<WeatherForecast[]>([]);

  useEffect(() => {
    const fetchWeatherData = async () => {
      try {
        const response1 = await fetch('http://95.140.152.201:5000/api/weather');
        const response2 = await fetch('http://localhost:5000/api/weather');
        const response3 = await fetch('http://127.0.0.1:5000/api/weather');
        const response4 = await fetch('http://95.140.152.201/api/weather');
        if (response1.ok) {
          const data = await response1.json();
          setWeatherForecasts(data);
          console.log('response1');
        } else {
          console.log('response1 Failed to fetch weather data');
        }
        if (response2.ok) {
          const data = await response2.json();
          setWeatherForecasts(data);
          console.log('response2');
        } else {
          console.log('response2 Failed to fetch weather data');
        }
        if (response3.ok) {
          const data = await response3.json();
          setWeatherForecasts(data);
          console.log('response3');
        } else {
          console.log('response3 Failed to fetch weather data');
        }
        if (response4.ok) {
          const data = await response4.json();
          setWeatherForecasts(data);
          console.log('response4');
        } else {
          console.log('response4 Failed to fetch weather data');
        }
      } catch (error) {
        console.error(error);
      }
    };

    fetchWeatherData();
  }, []);

  return (
    <div>
      <h1>Weather Forecasts</h1>
      <ul>
        {weatherForecasts.map((forecast) => (
          <li key={forecast.date}>
            <strong>{forecast.date}</strong>: {forecast.temperatureC}°C, {forecast.temperatureF}°F -{' '}
            {forecast.summary}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default Weather;

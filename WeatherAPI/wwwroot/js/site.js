const uri = 'api/geolocationdto';

$(document).ready(function () {
    postLocation();
});

function postLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(async position => {
            console.log(position);
            const latitude = position.coords.latitude;
            const longitude = position.coords.longitude;
            //document.getElementById('latitude').textContent = latitude;
            //document.getElementById('longitude').textContent = longitude;
            const data = { latitude, longitude };
            fetch(uri, {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            })
                .then(response => response.json())
                .then(() => {
                    getWeather();
                })
                .catch(error => console.error('oopsie poopsie!', error));
        });
    } else {
        console.log('geolocation not available')
    }

}

function getWeather() {

    fetch('api/geolocationdto/City')
        .then(response => response.json())
        .then(data => displayWeather(data))
        .catch(error => console.error('Unable to get things.', error));
}

function displayWeather(data) {

    const temp_f = Math.round(data.temp);
    const temp_c = Math.round((temp_f - 32) * (5 / 9));

    var id = data.id;
    var icon = data.icon;

    var flop_img = "url(./img/flop-img/" + icon + ".png)";
    $('.flop-image').css('background-image', flop_img);
    var sky_img = "url(./img/sky-img/" + icon + ".jpg)";
    $('.bgimg').css('background-image', sky_img);

    if (temp_c <= -30) {
        $('.temp').css('color', '#20A5FF')
    } else if (temp_c > 20 && temp_c <= 30) {
        $('.temp').css('color', '#DEDEDE')
    }

    document.getElementById('location').textContent = data.city;
    document.getElementById('temp').textContent = temp_f + '°F/' + temp_c + '°C';
    document.getElementById('summary').textContent = data.summary;
    console.log(data);
    console.log(data.id);
}
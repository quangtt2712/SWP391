const bar_chart = document.querySelector('#bar_chart')

const data_bike = {
    labels: ['Xe đạp 1', 'Xe đạp 2', 'Xe đạp 3', 'Xe đạp 4', 'Xe đạp 5', 'Xe đạp 6'],
    datasets: [{
        data: [10, 5, 20, 14, 18, 7],
        backgroundColor: [
            '#6D7EFF',
            '#6D7EFF',
            '#6D7EFF',
            '#6D7EFF',
            '#6D7EFF',
            '#6D7EFF'
        ]
    }]
}
const options_bike = {

    plugins: {
        legend: false
    },
    scales: {
        x: {
            grid: {
                display: false
            }
        },
        y: {
            min: 0,
            max: 30,
            ticks: {
                stepSize: 2,
                callback: value => value + 'K'
            },
            grid: {
                display: false
            }
        }
    }
}
new Chart(bar_chart, {
    type: 'bar',
    data: data_bike,
    options: options_bike
})
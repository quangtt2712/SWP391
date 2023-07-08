const ctx = document.querySelector('#line_chart')

const month = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];


fetch('/api/car/depositByMonth')
    .then(res => res.json())
    .then(arr => {
        console.log(arr)
        let arr_revenue = arr.map(item => item.totalDeposit)
        console.log(arr_revenue)
        return new Promise(res => res(arr_revenue))
    })
    .then(arr => {
        console.log(Math.max(...arr))
        const data = {
            labels: month,
            datasets: [
                {
                    data: arr,
                    backgroundColor: 'transparent',
                    borderColor: '#6D7EFF',
                    pointerBorderColor: 'transparent',
                    pointBorderWidth: 2

                }
            ]
        };

        const options = {

            plugins: {
                legend: false
            },
            scales: {
                x: {
                    grid: {
                        display: false
                    },


                },
                y: {
                    grid: {
                        display: false
                    },

                    min: Math.min(...arr),
                    ticks: {
                        // forces step size to be 50 units
                        stepSize: 100000
                    }

                },
              
            }
        }
        new Chart(ctx, {
            type: 'line',
            data: data,
            options: options
        })
    })

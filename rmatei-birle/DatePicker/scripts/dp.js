var c_day = new Date().getDate()+1
var c_month = new Date().getMonth()+1
var c_year = new Date().getFullYear()


function toggle_display(elem){
    if (elem.style.display == 'block') {
        elem.style.display = 'none'
    }
    else{
        elem.style.display = 'block'
        // if(date_validate(elem.value)){
        //     dt_split = elem.value.split('/')
        //     if(dt_split.length == 3) {
        //         c_day = parseInt(dt_split[0])
        //         c_month = parseInt(dt_split[1])
        //         c_year = parseInt(dt_split[2])
        //     }

        // }
        populate()
    }
}

function populate(){
    mnths = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec']
    document.getElementById('date_year').innerText = c_year
    document.getElementById('date_month').innerText = mnths[c_month-1]

    dates_list = []
    start_day = getDayOfMonthFirst(c_month, c_year)
    nr_days = getNrOfDaysInMonth(c_month, c_year)
    for(i=0; i<start_day; i++){
        dates_list.push(' ')
    }
    for(i=1; i<=nr_days; i++){
        dates_list.push(''+i)
    }
    for(i=0; i<5*7-start_day-nr_days; i++){
        dates_list.push(' ')
    }


    for(i=0; i<5; i++){
        for(j=0; j<7; j++){
            addressed_cell = document.getElementById('c'+i+j)
            addressed_cell.value = dates_list[7*i+j]
            if(addressed_cell.value == (''+c_day)){
                addressed_cell.style.background = 'lightgray'
            }
            else{
                addressed_cell.style.background = 'white'
            }
        }
    }
}
function setDate(self){
    text = ''
    c_day = parseInt(self.value)
    if(self.value != ' '){
        text = self.value+'-'+c_month+'-'+c_year
    }
    document.getElementById('date').value = text
    populate()
}

function getNrOfDaysInMonth(month, year){
    return new Date(year, month, 0).getDate()
}

function getDayOfMonthFirst(month, year){ //0 is sunday
    return new Date(year, month-1, 1).getDay()
}

function prev_year(){
    if(c_year>1900){
        c_year -= 1
        populate()
    }
}

function next_year(){
    if(c_year<2200){
        c_year += 1
        populate()
    }
}

function prev_month(){
    if(c_month>1){
        c_month -= 1
        populate()
    }
}

function next_month(){
    if(c_month<12){
        c_month += 1
        populate()
    }
}

function date_validate(dt){
    if(dt == '') return false
    dt_split = dt.split('-')
    if(dt_split.length != 3) return false

    d = parseInt(dt_split[0])
    m = parseInt(dt_split[1])
    y = parseInt(dt_split[2])

    if(d<1 || d>31) return false
    if(m<1 || m>12) return false
    if(y<1 || y>3000) return false

    return true
}


    // date = new Date()
    // if(date_validate(document.getElementById('date').value)){
    //     dt_split = document.getElementById('date').value.split('/')
    //     if(dt_split.length != 3) return false

    //     d = parseInt(dt_split[0])
    //     m = parseInt(dt_split[1])
    //     y = parseInt(dt_split[2])

    //     date = new Date(day=d, month=m,year=y)
    // }
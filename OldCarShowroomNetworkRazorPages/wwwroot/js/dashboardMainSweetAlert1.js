const handle_delete = () => {
    Swal.fire({
        title: 'Do you want to delete?',
        showDenyButton: true,
        confirmButtonText: 'Yes',
        denyButtonText: `Cancel`,
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire('Deleted!', '', 'success')
        } else if (result.isDenied) {
            Swal.fire('Canceled', '', 'error')
        }
    })
}
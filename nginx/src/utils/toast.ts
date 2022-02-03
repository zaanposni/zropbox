import { toast } from '@zerodevx/svelte-toast'

export function toastSuccess(msg: string) {
    toast.push(msg, {
        theme: {
          '--toastBackground': '#48BB78',
          '--toastBarBackground': '#2F855A'
        },
        pausable: true,
        duration: 3000
    });
}

export function toastWarning(msg: string) {
    toast.push(msg, {
        theme: {
          '--toastBackground': '#FF8000',
          '--toastBarBackground': '#A65C00'
        },
        pausable: true,
        duration: 5000
    });
}

export function toastError(msg: string) {
    toast.push(msg, {
        theme: {
            '--toastBackground': '#F56565',
            '--toastBarBackground': '#C53030'
        },
        pausable: true,
        duration: 5000
    });
}

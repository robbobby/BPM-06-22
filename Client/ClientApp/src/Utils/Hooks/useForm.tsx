import { useState } from "react";

export function useForm<T>(initialState: T) {
  const [form, setForm] = useState(initialState);

  const handleInputChange = (event: InputEvent) => {
    const target = event.target as HTMLInputElement;
    setForm({ ...form, [target.name]: target.value });
  }

  return { form, handleInputChange };
}

interface InputEvent {
    target: {
        name: string;
        value: any;
    }
}
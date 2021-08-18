

import { Photo } from './Photos';

export interface User {
	id: number;
	username: string;
	knownAs: string;
	age: number;
	gender: string;
	created: Date;
	lastAtice: Date;
	photoUrl: string;
	city: string;
	country: string;
	interests?: string;
	introduction?: string;
	lookingFor?: string;
	photos?: Photo[];
}
